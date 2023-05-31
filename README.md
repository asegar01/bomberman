# IAV - Proyecto Final


## Autor
- Alejandro Segarra Chacón. Usuario de GitHub: [asegar01](https://github.com/asegar01/IAV-SegarraChacon)

# Bomberman

## Propuesta
La práctica consiste en desarrollar un prototipo de Bomberman utilizando inteligencia artificial para controlar a los enemigos. El objetivo del juego es que el jugador, controlando al personaje principal, se enfrente a enemigos controlados por IA y trate de eliminarlos mediante la colocación estratégica de bombas.
Para los enemigos se usará un árbol de comportamiento.

## Punto de partida
El proyecto de Bomberman se ha desarrollado desde cero, aprovechando algunos recursos externos para acelerar el desarrollo de ciertos elementos del juego.

## Mecánicas del juego
Movimiento por celdas: El juego se desarrollará en un escenario basado en cuadrículas, donde tanto el jugador como los enemigos se moverán de manera discreta de una celda a otra. Se utilizará un grid para gestionar la posición y colisión de los personajes y obstáculos en el escenario.

Control del personaje principal: El jugador podrá controlar al personaje principal mediante el teclado. El personaje podrá moverse en las cuatro direcciones usando las teclas WASD y colocar bombas de manera estratégica con la tecla Espacio.

Enemigos controlados por IA: Habrá varios enemigos en el juego, cada uno con un árbol de comportamiento. Los enemigos utilizarán el árbol de comportamiento para tomar decisiones sobre su movimiento, colocación de bombas, búsqueda del jugador y evasión de explosiones. El objetivo de los enemigos será eliminar al jugador utilizando tácticas inteligentes.

Obstáculos: El jugador deberá enfrentarse a diferentes obstáculos en el escenario, como paredes o bloques destructibles.

## Diseño de la solución
El objetivo del enemigo es eliminar al jugador. Para ello, analizan constantemente la situación actual del juego haciendo uso de un árbol de comportamiento. Consideran factores como la ubicación del jugador, la existencia de obstáculos y la presencia de bombas. Con base en esta información, ajustan su comportamiento y toman decisiones estratégicas para aumentar sus posibilidades de eliminar al jugador.

![Captura de pantalla 2023-05-31 120749](https://github.com/asegar01/IAV-SegarraChacon/assets/82326232/86742c35-1e23-46f1-9513-36f75219af8a)

## Implementación de niveles
Se han implementado dos niveles en el juego para proporcionar una experiencia progresiva y desafiante al jugador.

### Nivel 1: 
Este nivel puede ser accedido a través del menú y sirve como introducción al juego ya que presenta un escenario relativamente sencillo. El jugador se enfrentará a un enemigo controlado por IA con comportamiento básico. Este nivel se centra en familiarizarse con los controles, las mecánicas básicas y la interacción con los obstáculos del escenario. El mapa es de tamaño moderado y cuenta con un número limitado de obstáculos destruíbles.

### Nivel 2: 
Puede ser accedido al completar el primer nivel y representa un aumento en la dificultad y un escenario más desafiante. El jugador se enfrentará a un enemigo controlado por IA con mayor capacidad de reacción. Además, el mapa es más grande y cuenta con una mayor cantidad de obstáculos destruíbles.

## Pruebas y métricas
https://youtu.be/-STMP8FfwtA

## Referencias

Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington.
- https://docs.unity3d.com/bolt/1.4/manual/index.html
