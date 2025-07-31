import { FastifyInstance } from "fastify";

export function initialize(fastify: FastifyInstance) {
    fastify.get("/");
}

async function addIPAddress(address: string, data: JsonObject) {
  const ipAddress = await prisma.ip.create({
    data: {
      address: address,
      data: data
    },
  });
  return ipAddress;
}