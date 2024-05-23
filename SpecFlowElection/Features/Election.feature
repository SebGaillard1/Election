Feature: Election

A short summary of the feature

@tag1
Scenario: Candidate wins with more than 50% of the vote in the first round
    Given a candidate has more than 50% of the vote
    And the first round of the election is closed
    When we check the result of the first round
    Then this candidate has won the election

  Scenario: Display vote counts and percentages at election close
    Given the election is closed
    And votes have been cast for multiple candidates
    When we check the result of the election
    Then the number of votes and percentages for each candidate are displayed

  Scenario: No candidate with over 50%, proceed to second round
    Given no candidate has more than 50% of the vote
    And the first round of the election is closed
    When we check the result of the first round
    Then the top two candidates should proceed to a second round

  Scenario: Determine winner in the second round with highest percentage
    Given candidates have contested the second round
    And the second round of the election is closed
    When we check the result of the second round
    Then the candidate with the highest percentage of votes wins the election

  Scenario: Tie in the second round, no winner declared
    Given candidates have the same percentage of votes in the second round
    And the second round of the election is closed
    When we check the result of the second round
    Then no winner is declared due to a tie
