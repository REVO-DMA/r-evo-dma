import zlib from "zlib";

export function inflate(data: zlib.InputType): Promise<Buffer | null>
{
    return new Promise((resolve) => {
        try
        {
            zlib.inflate(data, (err, res) => {
                if (err)
                {
                    console.error(err);
                    resolve(null);
                }

                resolve(res);
            });
        }
        catch (err)
        {
            console.error(err);
            resolve(null);
        }
    });
}

export function deflate(data: zlib.InputType): Promise<Buffer | null>
{
    return new Promise((resolve) => {
        try
        {
            zlib.deflate(data, (err, res) => {
                if (err)
                {
                    console.error(err);
                    resolve(null);
                }

                resolve(res);
            });
        }
        catch (err)
        {
            console.error(err);
            resolve(null);
        }
    });
}
