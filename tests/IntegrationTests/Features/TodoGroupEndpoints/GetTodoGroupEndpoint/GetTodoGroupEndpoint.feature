Feature: Get todo group endpoint
    As an end user
    I want to get a todo group
    So that I can view todo group detail.

  Background: 
    Given the following todo groups:
      | Id | Name   |
      |  1 | Marvel |
      |  2 | DC     |

  Scenario: Get todo group by Id
    When I send a GET request to "/api/todo-groups/1"
    Then the response should contain the todo group detail:
      | Id | Name   |
      |  1 | Marvel |
