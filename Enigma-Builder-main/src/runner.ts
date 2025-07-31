import workerFarm from 'worker-farm';

const service = workerFarm({
    maxCallsPerWorker: 1,
    maxCallTime: 60000 * 5, // 5 mins
    maxRetries: 3,
}, require.resolve('./worker.js'));

export function BuildProgram(args: string[]) {
    service(args, (err: any, output: any) => {});
}
