using PSS.amg538.Practica_02;

var cfg = Conecta4Factory.CargarConfiguracion("appsettings.json");
var juego = Conecta4Factory.CrearJuego(cfg);
var motor = new MotorConsola(juego, new Entrada(), new Salida(), cfg.Idioma);

motor.Ejecutar();