import { Result } from "@badrap/result";
import { InputType, deflate, inflate } from "zlib";

export function compress(buf: InputType) : Promise<Result<never, Error> | Result<Buffer, Error>>
{
    return new Promise((resolve, reject) => {
        deflate(buf, (err, result) => {
            if (err)
                resolve(Result.err(err));
    
            resolve(Result.ok(result));
        });
    });
}

export function decompress(buf: InputType) : Promise<Result<never, Error> | Result<Buffer, Error>>
{
    return new Promise((resolve, reject) => {
        inflate(buf, (err, result) => {
            if (err)
                resolve(Result.err(err));
    
            resolve(Result.ok(result));
        });
    });
}