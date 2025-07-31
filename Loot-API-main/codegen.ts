import { CodegenConfig } from '@graphql-codegen/cli';
import globals from "./src/globals";

const config: CodegenConfig = {
  schema: globals.loot.server,
  documents: ['src/**/*.ts'],
  ignoreNoDocuments: true,
  generates: {
    './src/gql/': {
      preset: 'client',
      plugins: [],
    }
  },
};

export default config;