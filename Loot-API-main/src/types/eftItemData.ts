import { Static, Type } from '@sinclair/typebox';

export const EftItemData = Type.Object({
  uri: Type.String({ format: 'uri' })
});

export type EftItemDataType = Static<typeof EftItemData>;