<?php
require('../session.php');

$session = get('s');
$stop = false;
$data = query(sprintf('select * from %s where id = %s', stable('session'), quote($session)));
if (count($data) == 0) {
  print('Error: Invalid session!');
  $stop = true;
}
$userid = $data[0]['user_id'];

$gameId = get('g');
$row = get('r');
$column = get('c');
$piece = get('p');

if (!$stop) {
  if ($piece != 1 && $piece != 2) {
    print('Error: Invalid piece!');
    $stop = true;
  }
}

if (!$stop) {
  $data = query(sprintf('select * from %s where id = %s and joined is not null and cancelled is null and finished is null', table('game'), quote($gameId)));
  if (count($data) == 0) {
    print('Error: Invalid game!');
    $stop = true;
  } else {
    if ($piece == 1) {
      if ($data[0]['first_player'] != $userid) {
         print('Error: Invalid player!');
         $stop = true;
      }
    }
    if ($piece == 2) {
      if ($data[0]['second_player'] != $userid) {
         print('Error: Invalid player!');
         $stop = true;
      }
    }
    if (!$stop) {
      if ($row < 0 || $row > $data[0]['board_height']) {
        print('Error: Invalid row!');
        $stop = true;
      }
    }
    if (!$stop) {
      if ($column < 0 || $column > $data[0]['board_width']) {
        print('Error: Invalid column!');
        $stop = true;
      }
    }
  }
}

$step = 0;
if (!$stop) {
  $data = query(sprintf('select * from %s where game_id = %s order by step desc limit 1', table('move'), quote($gameId)));
  if (count($data) == 0) {
    if ($piece != 1) {
      print('Error: Invalid piece!');
      $stop = true;
    }
  } else {
    if ($data[0]['piece'] == $piece) {
      print('Error: Invalid piece!');
      $stop = true;
    } else {
      $step = $data[0]['step'];
    }
  }
}

if (!$stop) {
  $id = uniqid();
  $step = $step + 1;
  execute(sprintf('insert into %s values (%s, %s, %s, %s, %s, %s)', table('move'), quote($id), quote($gameId), $step, $row, $column, $piece));
  print('Success');
}

?>