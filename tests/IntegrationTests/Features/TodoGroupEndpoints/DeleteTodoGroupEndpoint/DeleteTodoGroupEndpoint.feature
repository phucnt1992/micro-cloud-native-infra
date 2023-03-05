Feature: Delete todo group endpoint
  As an end user
  I want to delete a todo group
  So that I can remove an unused todo group

  Background: 
    Given the following todo groups:
      | Id | Name   |
      |  1 | Marvel |
      |  2 | DC     |
      |  3 | Anime  |

  Scenario: Delete a todo group without id should return method not allowed
    When I send a DELETE request to "/api/todo-groups/"
    Then the response status code should be 405

  Scenario: Delete a todo group should return success
    When I send a DELETE request to "/api/todo-groups/2"
    Then the response status code should be 204
    And the database should contain the following todo groups in any order:
      | Id | Name   |
      |  1 | Marvel |
      |  3 | Anime  |

  Scenario: Delete an unknown todo group should return not found
    When I send a DELETE request to "/api/todo-groups/4"
    Then the response status code should be 404

  Scenario: Delete a todo group with todo items should return success
    Given the following todo items:
      | Id | Title | DueDate    | State      | GroupId |
      |  1 | todo1 | 2022-03-22 | NotStarted |       1 |
      |  2 | todo2 |            | InProgress |       1 |
      |  3 | todo3 | 2022-02-01 | Completed  |       2 |
      |  4 | todo4 |            | NotStarted |         |
    When I send a DELETE request to "/api/todo-groups/1"
    Then the response status code should be 204
    And the database should contain the following todo groups in any order:
      | Id | Name  |
      |  2 | DC    |
      |  3 | Anime |
    And the database should contain the following todo items in any order:
      | Id | Title | DueDate    | State      | GroupId |
      |  3 | todo3 | 2022-02-01 | Completed  |       2 |
      |  4 | todo4 |            | NotStarted |         |
