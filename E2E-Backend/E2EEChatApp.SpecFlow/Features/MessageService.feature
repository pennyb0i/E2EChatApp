Feature: MessageService
	A service for sending and receiving messages

	Scenario: Send a message to an existing user
		Given the other user exists
		When I send a message to the other user
		Then the message is sent
		
	Scenario: Send a message to a nonexistent user
		Given the other user doesn't exist
		When I send a message to the other user
		Then an exception is thrown

	Scenario: Get messaging history with an existing user
		Given the other user exists
		When I fetch all messaging history with the other user
		Then the messaging history is returned
		
	Scenario: Get messaging history with a nonexistent user
		Given the other user doesn't exist
		When I fetch all messaging history with the other user
		Then an exception is thrown