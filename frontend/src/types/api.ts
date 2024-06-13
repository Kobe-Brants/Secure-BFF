import { UseQueryOptions } from 'react-query';

export type QueryOpt<Response> = UseQueryOptions<
  Response,
  any,
  Response,
  any[]
>;
