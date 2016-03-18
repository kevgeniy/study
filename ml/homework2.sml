fun all_except_option_fast(line, lines_list) =
	let
		fun all_except_option_tail(line, lines_list, line_find, picked_list) =
		case lines_list of
			[] => if line_find then SOME picked_list else NONE
			| head :: tail => if head = line
							then all_except_option_tail(line, tail, true, picked_list)
							else all_except_option_tail(line, tail, line_find, head::picked_list);
	in
	all_except_option_tail(line, lines_list, false, [])
	end

fun all_except_option(line, lines_list) =
	case lines_list of
	[] => NONE
	| head :: ln_list => case all_except_option(line, ln_list) of
						SOME result_list => SOME (if head = line then result_list else head :: result_list)
						| NONE => if head = line then SOME ln_list else NONE

fun get_substitutions1(list_list, line) =
	case list_list of
		[] => []
		| head_list :: list_list => case all_except_option(line, head_list) of
							SOME ls => ls @ get_substitutions1(list_list, line)
							| NONE => get_substitutions1(list_list, line)

fun get_substitutions2(list_list, line) =
	let
		fun get_substitutions_tail(line, list_list, result) =
			case list_list of
			[] => result
			| head_list :: list_list => case all_except_option(line, head_list) of
								SOME ls => get_substitutions_tail(line, list_list, ls @ result)
								| NONE => get_substitutions_tail(line, list_list, result)
	in
		get_substitutions_tail(line, list_list, [])
	end

fun similar_names(list_list, {first = x, middle = y, last = z}) =
	let
		fun do_name_full (names_list, result_list) =
		case names_list of
			[] =>result_list
			| head :: tail => do_name_full(names_list, {first = head, middle = y, last = z} :: result_list)
	in
		{first = x, middle = y, last = z} :: do_name_full(get_substitutions2(list_list, x), [])
	end

datatype suit = Clubs | Diamonds | Hearts | Spades
datatype rank = Jack | Queen | King | Ace | Num of int 
type card = suit * rank

datatype color = Red | Black
datatype move = Discard of card | Draw 

exception IllegalMove

fun card_color(suit, rank) =
	case suit of
		Hearts => Red
		| Diamonds => Red
		| _ => Black

fun card_value(suit, rank) =
	case rank of
		Num(value) => value
		| Ace => 11
		| _ => 10

fun remove_card (card_list, card, ex) =
	case card_list of
		[] => raise ex
		| head :: tail => if(head = card) then tail else head :: remove_card(tail, card, ex)

fun all_same_color(card_list) =
	case card_list of
		[] => true
		| _ :: [] => true
		| card :: card' :: tail => if card_color(card) = card_color(card') then all_same_color(card' :: tail) else false

fun sum_cards(card_list) =
	let 
		fun sum_cards_tail(card_list, acc) =
			case card_list of
				[] => acc
				| head :: tail => sum_cards_tail(tail, acc + card_value(head))
	in
		sum_cards_tail(card_list, 0)
	end

fun score(card_list, goal) =
	let
		val sum = sum_cards(card_list)
		val preliminary_score = if sum > goal then 3 * (sum - goal) else goal - sum
	in
		if all_same_color(card_list)
		then preliminary_score div 2
		else preliminary_score
	end
fun officiate(card_list, moves, goal) =
	let
		fun play_game(card_list, moves, held_list) =
			case moves of
				[] => score(held_list, goal)
				| Discard(card) :: moves_tail => 
					play_game(card_list, moves_tail, remove_card(held_list, card, IllegalMove))
				| Draw :: moves_tail => 
					case card_list of
						[] => score(held_list, goal)
						| head :: tail => 
							if sum_cards(head :: held_list) > goal 
							then score(head :: held_list, goal) 
							else  play_game(tail, moves_tail, head::held_list)
	in
		play_game(card_list, moves, [])
	end
fun score_challenge(card_list, goal) =
	let 
		fun count_card(card_list, card, acc) =
			case card_list of
			[] => acc
			| (suit, rank) :: tail => if rank = card then count_card(tail, card, acc + 1)
							else count_card(tail, card, acc)
		val sum = score(card_list, goal)
		val preliminary_score = if sum < goal then goal - sum
							else
								let
									val ace_count = count_card(card_list, Ace, 0)
								in
								if sum - goal <= 10 * ace_count then 0 
								else 3 * (sum - goal - ace_count * 10)
								end
	in
		if all_same_color(card_list)
		then preliminary_score div 2
		else preliminary_score
	end

fun officiate_challenge(card_list, moves, goal) =
	let
		fun score(card_list, goal) = score_challenge(card_list, goal)
	in
		officiate(card_list, moves, goal)
	end

fun careful_player(card_list, goal) = 
	let
		fun play_game(card::tail, goal, held_list) =
			let
				val sum = score(held_list, goal)
			in
			if sum < goal - 10
			then Draw :: play_game(tail, goal, card :: held_list)
			else if sum = goal
				then []
				else if sum < goal andalso score(card::held_list, goal) < goal
				then Draw :: play_game(tail, goal, card::held_list)
				else Discard(card) :: play_game(tail, goal, held_list)
			end
	in
		play_game(card_list, goal, [])
	end
