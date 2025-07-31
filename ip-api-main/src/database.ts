import { PrismaClient } from "@prisma/client";
import { JsonObject } from "@prisma/client/runtime/library";

let prisma: PrismaClient;

export async function initialize() {
    prisma = new PrismaClient();
    await prisma.$connect();
}

export async function shutdown() {
    await prisma?.$disconnect();
}