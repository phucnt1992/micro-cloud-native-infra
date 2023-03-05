Feature: Get todo item endpoint
  As an end user
  I want to get a todo item
  So that I can view a todo item detail

  Background: 
    Given a todo item with id 1 exists

  Scenario: Get todo item without id should return method not allowed
    When I send a GET request to "/api/todo-items"
    Then the response status code should be 405

  Scenario: Get todo item by id should return success
    When I send a GET request to "/api/todo-items/1"
    Then the response status code should be 200
    And the response body should contain todo item with id 1

  Scenario: Get todo item by id that does not exist should return not found
    When I send a GET request to "/api/todo-items/999"
    Then the response status code should be 404
