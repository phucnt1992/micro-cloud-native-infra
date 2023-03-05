Feature: Create todo item endpoint
  As an end user
  I want to be able to create a todo item
  So that I can add a todo item to my todo list

  Scenario: Create todo item should return success
    Given There is no todo item with id 1 in database
    When I send a POST request to "/api/todo-items" with the todo item data
    Then the response status code should be 201
    And the response header contains "Location" with value "/api/todo-items/1"
    And the database should contain created todo item with id 1

  Scenario: Create todo item with invalid data should return bad request
    When I send a POST request to "/api/todo-items" with the invalid todo item data
    Then the response status code should be 400

  Scenario: Create todo item with group should return success
    Given I have a todo group with id 1
    When I send a POST request to "/api/todo-items" with the todo item data contain group id 1
    Then the response status code should be 201
    And the response header contains "Location" with value "/api/todo-items/1"
    And the database should contain created todo item with id 1

  Scenario: Create todo item with invalid todo group should return bad request
    Given I have a todo group with id 1
    When I send a POST request to "/api/todo-items" with the todo item data contain group id 2
    Then the response status code should be 400
