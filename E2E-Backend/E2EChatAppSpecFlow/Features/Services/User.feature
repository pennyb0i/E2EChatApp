Feature: User Service Testing

    Scenario: Get all users
        Given I am a logged-in user with ID "<currentUserId>"
        When I request all users with friendsOnly set to "<friendsOnly>"
        Then I should receive a list of users
        
    Examples:
      | currentUserId | friendsOnly |
      | 1             | true        |
      | 2             | true        |

    Scenario: Get a user by ID
        Given I have a user ID "<userId>"
        When I request user details for that ID
        Then I should receive the user details
        
    Examples:
      | userId |
      | 1      |
      | 2      |
      