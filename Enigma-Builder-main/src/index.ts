import Fastify from 'fastify';
import { AuthWrapper } from './auth/authWrapper';
import { JsonSchemaToTsProvider } from '@fastify/type-provider-json-schema-to-ts';
import { BuildProgram } from "./runner";
import sanitize from 'sanitize-filename';
import { v4 as generateUUID, validate as validateUUID } from 'uuid';
import cron from 'node-cron';
import { DateTime } from 'luxon';
import fse from "fs-extra";
import path from 'path/win32';
import { ValidRebrands } from "./validRebrands";

const cleanupIntervalMins = 5;
const queueEntryMaxAgeMins = 5;

interface QueueItem {
  timestamp: DateTime;
}

fse.emptyDirSync(path.join(__dirname, "../output"));

const builderQueue = new Map<string, QueueItem>();

cron.schedule(`*/${cleanupIntervalMins} * * * *`, async () => {
  console.log("[i] Running cleanup...");

  let cleanedEntities = 0;

  for (const entry of builderQueue) {
    const key = entry[0];
    const value = entry[1];

    const ageInMinutes = value.timestamp.diffNow().milliseconds;

    if (Math.abs(ageInMinutes) >= queueEntryMaxAgeMins * 60000) {
      await fse.rm(path.join(__dirname, `../output/${key}.exe`), { force: true });
      builderQueue.delete(key);
      cleanedEntities++;
    }
  }

  console.log(`[i] Cleaned up ${cleanedEntities} entities!`);
});

const fastify = Fastify({
  logger: false
}).withTypeProvider<JsonSchemaToTsProvider>();

(async () => {
  await fastify.register(import('fastify-ip'), {
    strict: false,
    isAWS: false,
  });
  
  await fastify.register(import('@fastify/rate-limit'), {
    global: false,
    timeWindow: '1 minute'
  });

  await fastify.register(import('@fastify/static'), {
    root: path.join(__dirname, '../output'),
    prefix: '/builds/',
    index: false,
    list: false,
    decorateReply: false,
  });

  fastify.post('/generate', {
    schema: {
      body: {
        type: 'object',
        properties: {
          licenseKey: { type: 'string' },
          hwid: { type: 'string' },
          hwid2: { type: 'string' },
          commandLine: { type: 'string' },
          auth_luk: { type: 'string' },
        },
        required: [
          'licenseKey',
          'hwid',
          'hwid2',
          'commandLine',
          'auth_luk'
        ]
      },
      response: {
        200: {
          description: "Successful generate response",
          type: 'object',
          properties: {
            success: { type: 'boolean' },
            message: { type: 'string' }
          }
        }
      }
    },
    config: {
      rateLimit: {
        max: 2
      }
    }
  }, async (request, reply) => {
    const { licenseKey, hwid, hwid2, commandLine, auth_luk } = request.body;

    console.log(`[/generate] Handling a req from "${request.ip}"`);
    console.log(`\tLicense Key    -> ${licenseKey}`);
    console.log(`\tKeyauth HWID   -> ${hwid}`);
    console.log(`\tEnigma HWID    -> ${hwid2}`);
    console.log(`\tCommand Line   -> ${commandLine}`);
    console.log(`\tAuth LUK       -> ${auth_luk}`);

    // Try to get the rebrand from this look up key
    const rebrand = ValidRebrands.get(auth_luk);
    if (rebrand === undefined) return reply.send({ success: false, message: "Invalid data" });

    const ka = new AuthWrapper(licenseKey, hwid, rebrand);
    const isGoodUser = await ka.IsGoodUser();

    if (isGoodUser) {
      const jobID = generateUUID();

      builderQueue.set(jobID, {
        timestamp: DateTime.now()
      });

      BuildProgram([
        path.join(__dirname, "../bin/enigmaProtector/enigma64.exe"),
        path.join(__dirname, "../bin/enigmaProtector/template.enigma64"),
        path.join(__dirname, "../bin/input/flashTool.exe"),
        path.join(__dirname, `../output/${jobID}.exe`),
        hwid2,
        commandLine
      ]);

      return reply.send({ success: true, message: jobID });
    } else {
      return reply.send({ success: false, message: "Invalid data" });
    }
  });

  fastify.post('/check', {
    schema: {
      body: {
        type: 'object',
        properties: {
          jobID: { type: 'string' },
        },
        required: ['jobID']
      },
      response: {
        200: {
          description: "Successful check response",
          type: 'object',
          properties: {
            success: { type: 'boolean' },
            message: { type: 'string' }
          }
        }
      }
    },
    config: {
      rateLimit: {
        max: 30
      }
    }
  }, async (request, reply) => {
    const { jobID } = request.body;

    console.log(`[/check] Handling a req from "${request.ip}"`);
    console.log(`\tJob ID     -> ${jobID}`);

    const fileName = sanitize(jobID);
    if (!validateUUID(fileName)) {
      return reply.send({ success: false, message: "Invalid job ID" });
    }

    const queueItem = builderQueue.get(fileName);

    if (queueItem === undefined) {
      return reply.send({ success: false, message: "Invalid job ID" });
    }

    const filePath = path.join(__dirname, `../output/${fileName}.exe`);
    const fileExists = await fse.pathExists(filePath);

    if (fileExists) {
      return reply.send({ success: true, message: "complete" });
    } else {
      return reply.send({ success: true, message: "pending" });
    }
  });
  
  fastify.listen({ port: 8080 }, (err, address) => {
    if (err) {
      console.error(err);
      process.exit(1);
    }
  });
})();
