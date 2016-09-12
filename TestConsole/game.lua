-- Called by main Game loop. This is one turn of one player.
-- The current player is accessible through "TurnPlayer".
addEventHandler ("turn", function (event)
	triggerEvent ("draw-phase")
    triggerEvent ("standby-phase")
    triggerEvent ("main-phase-1")
    
    if TurnPlayer:AskForBattlePhase() then
        triggerEvent ("battle-phase")
        triggerEvent ("main-phase-2")
    end
    
    triggerEvent ("end-phase")
end)

-- The draw phase. Per default, only do a normal draw. This
-- should be the only place a normal draw is possible.
addEventHandler ("draw-phase", function (event)
	triggerEvent ("normal-draw")
end)

-- A normal draw is just using the draw action.
addEventHandler ("normal-draw", function (event)
	triggerEvent ("draw")
end)

-- Drawing is adding a card from the deck
addEventHandler ("draw", function (event)
	local deck = TurnPlayer.Deck
	if deck.Count == 0 then
		debug ("player cannot draw, deck is empty - player looses")
		TurnPlayer.LifePoints = 0
	else
		debug ("deck has " .. deck.Count .. " elements")
		triggerEvent("add", event:WithSourceLocation ("deck"))
	end
end)

addEventHandler ("add", function (event)
	assert (event:HasSourceLocation (), "Must have a location to add a card from")
	triggerEvent ("move", event:WithTargetLocation ("hand"))
end)

addEventHandler ("normal-set", function (event)
    assert (not TurnPlayer.HasNormalSet, "May only normal set once per turn")
    assert (event.HasSubjectCard, "Must have a card to set")
    assert (event.SubjectCard.IsMonster, "Only a monster can be normal set")
    triggerEvent ("set", event)
end)

addEventHandler ("set", function (event)
    assert (event:HasSubjectCard (), "Must have a card to set")
    assert (event:HasTargetLocation (), "Must have a target to set the card to")
    triggerEvent ("move", event)
end)

addEventHandler ("flip", function (event)
    assert (event:HasSubjectCard (), "Must have a card to flip")
    assert (event:SubjectCard.IsOnField, "Card to flip must be on field")
    triggerEvent ("activate", event)
end)