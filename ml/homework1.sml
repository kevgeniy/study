fun is_older(first : int * int * int, second : int * int * int) =
  if #1 first < #1 second
  then true
  else 
    if #1 first = #1 second andalso #2 first < #2 second
    then true
    else
      if #2 first = #2 second andalso #3 first < #3 second
      then true
      else false
  
fun number_in_month(dates : (int * int * int) list, month : int) =
  if null dates
  then 0
  else
      if #2 (hd dates) = month
      then 1 + number_in_month(tl dates, month)
      else number_in_month(tl dates, month)
      
fun number_in_months(dates : (int * int * int) list, months : int list) =
        if null months
        then 0
        else number_in_month(dates, hd months) + number_in_months(dates, tl
        months)

fun dates_in_month(dates : (int * int * int) list, month : int) =
  if null dates
  then []
  else
    if #2 (hd dates) = month
    then hd dates :: dates_in_month(tl dates, month)
    else dates_in_month(tl dates, month)

fun dates_in_months(dates : (int * int * int) list, months : int list) =
  if null months
  then []
  else dates_in_month(dates, hd months) @ dates_in_months(dates, tl months)

fun get_nth(lines : string list, n : int) =
  if n = 1
  then hd lines
  else get_nth(tl lines, n - 1)

fun date_to_string (date : int * int * int) =
  let 
    val months = ["January", "February", "March", "April", "May", "June", "July",
    "August", "September", "October", "November", "December"]
  in
    get_nth(months, #2 date) ^ " " ^ Int.toString(#3 date) ^ ", " ^
    Int.toString(#1 date)
  end

fun number_before_reaching_sum(sum : int, numbers : int list) =
  if sum <= 0
  then ~1
  else 1 + number_before_reaching_sum(sum - hd numbers, tl numbers)
fun what_month (day : int) =
  let 
    val days_in_month = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
  in
    number_before_reaching_sum(day, days_in_month) + 1
  end
fun month_range(day1 : int, day2 : int) =
  if day1 >  day2
  then []
  else what_month(day1) :: month_range(day1 + 1, day2)

fun oldest(dates : (int * int * int) list) =
  if null dates
  then NONE
  else
    let
      val next = oldest(tl dates)
    in
      if isSome(next) andalso is_older(valOf(next), hd dates)
      then next
      else SOME(hd dates)
    end

fun clean_list(months : int list) =
let
  fun is_in_list(elements : int list, element : int) =
    if null elements
    then false
    else 
      if (hd elements) = element
      then true
      else is_in_list(tl elements, element);
in
  if null months
  then []
  else
    if is_in_list(tl months, hd months)
    then clean_list(tl months)
    else (hd months) :: clean_list(tl months)
end

fun number_in_months_challenge(dates : (int * int * int) list, months : int list) =
  number_in_months(dates, clean_list(months))

fun dates_in_months_challenge(dates : (int * int * int) list, months : int list)
  = dates_in_months(dates, clean_list(months))

fun reasonable_date(date : int * int * int) = 
let
  fun is_day_of_month(date : int * int * int) =
  let        
    val days_in_month = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    fun is_leap(year : int) =
      year mod 400 = 0 orelse year mod 4 = 0 andalso year mod 100 <> 0
    fun get_nth(lines : int list, n : int) =
      if n = 1
      then hd lines
      else get_nth(tl lines, n - 1)
    val number_of_days = get_nth(days_in_month, #2 date)
  in
    #3 date > 0 andalso #3 date <= number_of_days orelse #2 date = 2 andalso 
    is_leap(#1 date) andalso #3 date <= (number_of_days + 1)
  end
in 
  #1 date > 0 andalso #2 date > 0 andalso #2 date < 12 andalso is_day_of_month(date)
end

