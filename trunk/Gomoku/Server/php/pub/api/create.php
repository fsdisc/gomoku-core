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

$playFirst = get('p');
$width = get('w');
$height = get('h');
$id = uniqid();

if (!$stop) {
  if ($width < 10 || $width > 255) {
    print('Error: Board width must be in [10; 255] range!');
    $stop = true;
  }
}

if (!$stop) {
  if ($height < 10 || $height > 255) {
    print('Error: Board height must be in [10; 255] range!');
    $stop = true;
  }
}

if (!$stop) {
  if ($playFirst == 1) {
    execute(sprintf("insert into %s values (%s, %s, '', sysdate(), NULL, NULL, NULL, 3, %s, %s)", table('game'), quote($id), quote($userid), $width, $height));
  } else {
    execute(sprintf("insert into %s values (%s, '', %s, sysdate(), NULL, NULL, NULL, 3, %s, %s)", table('game'), quote($id), quote($userid), $width, $height));
  }
  print('Success: ' . $id);
}

?>