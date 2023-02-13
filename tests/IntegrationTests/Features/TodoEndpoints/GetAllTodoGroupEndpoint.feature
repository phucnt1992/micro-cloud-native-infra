Feature: Get All Todo Group Endpoint
    As an end user
    I want to get all todo groups
    So that I can view all todo groups.

  Background: 
    Given the following todo groups:
      | Id | Name   |
      |  1 | Marvel |
      |  2 | DC     |
      |  3 | Anime  |

  Scenario: Get All Todo Groups
    When I send a GET request to "/api/todo-groups"
    Then the response should contain the following todo groups in any order:
      | Id | Name   |
      |  1 | Marvel |
      |  2 | DC     |
      |  3 | Anime  |
