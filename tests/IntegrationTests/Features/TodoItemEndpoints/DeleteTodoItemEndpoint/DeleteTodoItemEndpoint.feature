Feature: Delete todo item endpoint
  As an end user
  I want to delete a todo item
  So that I can remove items I no longer need

  Background: 
    Given there are 3 todo items exist in database

  Scenario: Delete todo item without id should return method not allowed
    When I send a DELETE request to "/api/todo-items"
    Then the response status code should be 405

  Scenario: Delete todo item should return success
    When I send a DELETE request to "/api/todo-items/1"
    Then the response status code should be 204
    And the todo item with id 1 should not exist in database

  Scenario: Delete todo item that does not exist should return not found
    When I send a DELETE request to "/api/todo-items/999"
    Then the response status code should be 404
