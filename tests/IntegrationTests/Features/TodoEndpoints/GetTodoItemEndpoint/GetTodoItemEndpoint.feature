Feature: Get todo item endpoint
  As an end user
  I want to delete a todo group
  So that I can remove an unused todo group

  Background: 
    Given a todo item with id 1 exists

  Scenario: Get todo item by id
    When I send a GET request to "/api/todo-items/1"
    Then the response status code should be 200
    And the response body should contain todo item with id 1

  Scenario: Get todo item by id that does not exist
    When I send a GET request to "/api/todo-items/999"
    Then the response status code should be 404
