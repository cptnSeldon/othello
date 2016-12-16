# Othello 

## Specifications
__Language__ : C# -> WPF

### Basic requirements

* Interface
  * __Board interface__ : XAML
  * __Score__           : synchronization (autobinding)
  * __Chronometer__     : turn time
  * __Dimensions__      : 800x600 by default (and minimum), yet resizable
* Menubar
  * __Save/ load__ game 
* Game
  * __Two players on the same computer__
  * __No AI__ required __yet__
  * __Only valid moves are authorized__ :
    * If valid -> authorized
    * If not valid -> player passes their turn
    * If no more valid moves -> end of game
  * __Playable tiles__  : sight of them before actual placement

### Nice-to-have

* Interface
  * Discs' __animation__
  * __Look & feel__ customizations
* Menubar
  * __Undo__
  * Play with friends (__network__)
* Game
  * (Sound/ music)
  * ('Mock' score is shown on the tiles before a move has been done)

## Project architecture
__TODO__
