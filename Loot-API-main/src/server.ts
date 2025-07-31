import fastify from 'fastify';
import { TypeBoxTypeProvider } from '@fastify/type-provider-typebox';
import { Auth, AuthType } from './types/auth';
import { EftItemData, EftItemDataType } from './types/eftItemData';
import { minutesToSeconds, signUrl } from './bunnySigner';
import { randomUUID } from "crypto";

const server = fastify().withTypeProvider<TypeBoxTypeProvider>();
server.register(require("fastify-ip"));

server.post<{ Body: AuthType, Reply: EftItemDataType }>(
    '/eft-item-data',
    {
      schema: {
        body: Auth,
        response: {
          200: EftItemData
        },
      },
    }, (request, reply) => {
      const { session_token } = request.body;
      //const ip = request.ip;

      // TODO: verify session

      const cacheBuster = randomUUID();
      const uri = signUrl(`https://loot.evodma.com/eft/itemData.evo?cacheBuster=${cacheBuster}`, minutesToSeconds(10));

      reply.status(200).send({ uri });
    }
);

export function start()
{
    server.listen({ port: 8080 }, (err, address) => {
        if (err)
        {
            console.error(err)
            process.exit(1)
        }

        console.log(`Server listening at ${address}`)
    });
}