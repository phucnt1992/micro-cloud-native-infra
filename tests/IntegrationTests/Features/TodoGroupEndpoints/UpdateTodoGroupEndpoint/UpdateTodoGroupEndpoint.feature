Feature: update todo group endpoint
  As an end user
  I want to update a todo group detail
  So that I can update the todo group detail

  Background: 
    Given the following todo groups:
      | Id | Name   |
      |  1 | Marvel |
      |  2 | DC     |
      |  3 | Anime  |

  Scenario: Update Todo Group
    When I send a PUT request to "/api/todo-groups/2" with the following data:
      | Name        |
      | Warner Bros |
    Then the response status code should be 200
    Then the response should contain the updated todo group detail:
      | Id | Name        |
      |  2 | Warner Bros |
    And the database should contain the following todo groups in any order:
      | Id | Name        |
      |  1 | Marvel      |
      |  2 | Warner Bros |
      |  3 | Anime       |
