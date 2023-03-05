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

  Scenario: Delete a todo group
    When I send a DELETE request to "/api/todo-groups/2"
    Then the response status code should be 204
    And the database should contain the following todo groups in any order:
      | Id | Name   |
      |  1 | Marvel |
      |  3 | Anime  |

  Scenario: Delete an unknown todo group
    When I send a DELETE request to "/api/todo-groups/4"
    Then the response status code should be 404
