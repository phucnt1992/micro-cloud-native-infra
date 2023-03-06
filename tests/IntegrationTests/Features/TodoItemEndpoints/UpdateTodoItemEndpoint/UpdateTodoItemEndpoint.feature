Feature: Update todo item endpoint
  As an end user
  I want to update a todo item
  So that I can change the status of a todo item

  Background:
    Given There is a todo item with id 1 exists

  Scenario: Update todo item should success
    When I send a PUT request to "/api/todo-items/1" with the todo item data
    Then the response status code should be 200
    And the response body should contain the updated todo item with id 1
    And the database should contain a todo item with id 1 and updated data

  Scenario: Update todo item with group id should success
    Given There is a todo group with id 1 exists
    When I send a PUT request to "/api/todo-items/1" with the todo item with group id 1 data
    Then the response status code should be 200
    And the response body should contain the updated todo item with id 1
    And the database should contain a todo item with id 1 and updated data

  Scenario: Update todo item with unknown id should return not found
    When I send a PUT request to "/api/todo-items/2" with the todo item data
    Then the response status code should be 404

  Scenario: Update todo item with invalid data should return bad request
    When I send a PUT request to "/api/todo-items/1" with invalid data
    Then the response status code should be 400

  Scenario: Update todo item with unknown group id should return bad request
    When I send a PUT request to "/api/todo-items/1" with the todo item data and unknown group id
    Then the response status code should be 400
