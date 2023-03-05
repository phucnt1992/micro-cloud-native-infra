Feature: Create todo group endpoint
  As an end user
  I want to create a todo group
  So that I can group my todos

  Scenario: Create Todo Group
    When I send a POST request to "/api/todo-groups" with following data:
      | Name   |
      | Marvel |
    Then the response status code should be 201
    And the response header contains "Location" with value "/api/todo-groups/1"
    And the database contains the following todo groups:
      | Id | Name   |
      |  1 | Marvel |

  Scenario: Create Todo Group with empty name
    When I send a POST request to "/api/todo-groups" with following data:
      | Name |
      |      |
    Then the response status code should be 400
