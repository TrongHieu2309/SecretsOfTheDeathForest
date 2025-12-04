### GAME: Secrets of the Death Forest
### Link: https://tronghieu2309.itch.io/hero

### Camera
- Điều khiển camera follow người chơi.

### Entity
- Lớp cha của **PlayerController** và **EnemyController**.

### Enemy
- Lớp cha **EnemyController** có các lớp con:
  - Boss
  - FlyEnemy
  - Hyena
  - Mushroom
  - Scorpion
  - Skeleton
  - Snake

### GameManager
- **CoinCollision**: Xử lý va chạm với đồng xu.
- **GameManager**: Xử lý các tính năng chung của game: UI, Energy, Event Animation.
- **MainMenu**: Xử lý giao diện Menu.
- **Parallax**: Xử lý chuyển động của background.
- **PickupEnergy**: Xử lý nhặt viên năng lượng.
- **SceneController**: Xử lý chuyển đổi giữa các Scenes.
- **SoundManager**: Xử lý âm thanh game.

### Player
- **PlayerCollision**: Xử lý va chạm với người chơi.
- **PlayerController**: Xử lý logic của người chơi.

### Selection
- Xử lý giao diện chọn nhân vật / màn chơi.
