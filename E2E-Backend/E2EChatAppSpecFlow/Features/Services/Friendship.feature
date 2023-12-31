Feature: FriendService
A service for creating, accepting, and cancelling friendships

    Scenario Outline: Create and accept Friendships
        Given there <IsFriendship> a friendship between me and the other user, initiated by <FriendshipMaker>, and the friendship <IsPending> pending
        When <Sender> sends a create request to <Receiver>
        Then the result is: <Result>

        Examples:
          | IsFriendship | FriendshipMaker | IsPending | Sender     | Receiver   | Result   |
          | is not       |                 |           | my user    | other user | Created  |
          | is not       |                 |           | other user | my user    | Created  |
          | is           | my user         | is        | my user    | other user | Nothing  |
          | is           | my user         | is not    | my user    | other user | Nothing  |
          | is           | other user      | is        | my user    | other user | Accepted |
          | is           | other user      | is not    | my user    | other user | Nothing  |
          | is           | my user         | is        | other user | my user    | Accepted |
          | is           | my user         | is not    | other user | my user    | Nothing  |
          | is           | other user      | is        | other user | my user    | Nothing  |
          | is           | other user      | is not    | other user | my user    | Nothing  |