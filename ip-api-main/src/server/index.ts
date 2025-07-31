import Fastify, { FastifyInstance } from "fastify";

const port = process.env.PORT || 5000;

let fastify : FastifyInstance;

export async function initialize() {
    fastify = Fastify({
        logger: true,
    });

    await fastify.listen({
        port: port as number,
        host: "0.0.0.0"
    });
}