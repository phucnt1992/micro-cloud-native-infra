import { kebabCase, snakeCase } from './string-utils';

describe('string-utils', () => {
  describe('kebabCase', () => {
    it('should convert a string to kebab case', () => {
      expect(kebabCase('Hello World')).toEqual('hello-world');
      expect(kebabCase('Hello World!')).toEqual('hello-world!');
    });
  });

  describe('snakeCase', () => {
    it('should convert a string to snake case', () => {
      expect(snakeCase('Hello World')).toEqual('hello_world');
      expect(snakeCase('Hello World!')).toEqual('hello_world!');
    });
  });
});
