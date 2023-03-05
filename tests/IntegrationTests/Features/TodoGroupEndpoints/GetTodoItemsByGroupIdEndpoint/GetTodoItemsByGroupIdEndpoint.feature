Feature: Get todo items by group id endpoint
    As an end user
    I want to get todo items by group id
    So that I can view all todo items belong to group id.

  Background: 
    Given the following todo groups:
      | Id | Name   |
      |  1 | group1 |
      |  2 | group2 |
    And the following todo items:
      | Id | Title | DueDate    | State      | GroupId |
      |  1 | todo1 | 2022-03-22 | NotStarted |       1 |
      |  2 | todo2 |            | InProgress |       1 |
      |  3 | todo3 | 2022-02-01 | Completed  |       2 |
      |  4 | todo4 |            | NotStarted |         |

  Scenario: Get todo items by group id
    When I send a GET request to "/api/todo-groups/1/items"
    Then the response status code should be 200
    And the response should contain the following in any order:
      | Id | Title | DueDate    | State      | GroupId |
      |  1 | todo1 | 2022-03-22 | NotStarted |       1 |
      |  2 | todo2 |            | InProgress |       1 |

  Scenario: Get todo items by group id with invalid group id
    When I send a GET request to "/api/todo-groups/999/items"
    Then the response status code should be 404
