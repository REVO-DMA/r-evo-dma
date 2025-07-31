import { Item as LootItem } from "../gql/graphql";
import { Readable } from "stream";

const CURRENCY_FORMATTER = new Intl.NumberFormat("en-RU", {
    style: "currency",
    currency: "RUB",
    notation: "compact",
    compactDisplay: "short",
    currencyDisplay: "code",
    minimumFractionDigits: 0,
    maximumFractionDigits: 1,
});

export function formatPrice(price: number): string
{
    try
    {
        return CURRENCY_FORMATTER.format(price).replace("RUB", "").trimStart();
    }
    catch
    {
        return "";
    }
}

export function shouldSkipItem(item: LootItem): boolean
{
    if (item.name == null ||
        item.shortName == null ||
        item.sellFor == null
    )
    {
        return true;
    }

    return false;
}

export function bufferToStream(data: Buffer)
{
    const stream = new Readable({
        read() {
            this.push(data);
            this.push(null);
        }
    });

    return stream;
}

export function buildDate(fileName: boolean): string
{
    const date = new Date();
    const dateStr = `${date.getMonth() + 1}-${date.getDate()}-${date.getFullYear()}`;

    if (fileName)
        return `${dateStr}.evo`;
    
    return dateStr;
}