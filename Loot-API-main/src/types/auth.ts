import { Static, Type } from '@sinclair/typebox';

export const Auth = Type.Object({
  session_token: Type.String({ format: 'uuid' })
});

export type AuthType = Static<typeof Auth>;