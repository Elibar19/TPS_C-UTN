using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

class Program
{
    static void Main()
    {
        List<sistemaEspacial> sistemas = new List<sistemaEspacial>();
        Random random = new Random();

        while (true)
        {
            Console.WriteLine("Menú:");
            Console.WriteLine("1. Simular un sistema y generar asteroides");
            Console.WriteLine("2. Procesar asteroides del sistema");
            Console.WriteLine("3. Salir del sistema y generar un reporte");
            Console.WriteLine("4. Salir del programa y generar un reporte general");
            Console.WriteLine("5. Salir");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    sistemas.Add(sistemaEspacial.generarSistemaRandom(random));
                    Console.WriteLine("Sistema generado con asteroides aleatorios.");
                    break;
                case "2":
                    if (sistemas.Count > 0)
                    {
                        sistemaEspacial sistemaActual = sistemas.Last();
                        sistemaActual.procesarAsteroide();
                        Console.WriteLine("Asteroides procesados en el sistema actual.");
                    }
                    else
                    {
                        Console.WriteLine("No hay sistemas generados para procesar asteroides.");
                    }
                    break;
                case "3":
                    if (sistemas.Count > 0)
                    {
                        sistemaEspacial sistemaActual = sistemas.Last();
                        sistemaActual.generarReporte();
                        sistemas.Remove(sistemaActual);
                        Console.WriteLine("Reporte del sistema generado.");
                    }
                    else
                    {
                        Console.WriteLine("No hay sistemas generados para generar un reporte.");
                    }
                    break;
                case "4":
                    generarReporteGeneral(sistemas);
                    return;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opción no válida. Intente nuevamente.");
                    break;
            }
        }
    }

    static void generarReporteGeneral(List<sistemaEspacial> sistemas)
    {
        int totalHierro = 0;
        int totalOro = 0;
        int totalPlatino = 0;
        int totalMiscelaneos = 0;
        int totalCarga = 0;

        foreach (var sistema in sistemas)
        {
            totalHierro += sistema.cantidadHierro;
            totalOro += sistema.cantidadOro;
            totalPlatino += sistema.cantidadPlatino;
            totalMiscelaneos += sistema.cantidadMiscelaneo;
            totalCarga += sistema.cantidadTotal;
        }

        Console.WriteLine("Reporte General:");
        Console.WriteLine($"Total de sistemas procesados: {sistemas.Count}");
        Console.WriteLine($"Total de carga procesada: {totalCarga} KG");
        Console.WriteLine($"Total de hierro: {totalHierro} KG");
        Console.WriteLine($"Total de oro: {totalOro} KG");
        Console.WriteLine($"Total de platino: {totalPlatino} KG");
        Console.WriteLine($"Total de metales misceláneos: {totalMiscelaneos} KG");
    }
}

class sistemaEspacial
{
    public string codigoSistema { get; }
    public List<Asteroide> Asteroides { get; }
    public int cantidadHierro { get; private set; }
    public int cantidadOro { get; private set; }
    public int cantidadPlatino { get; private set; }
    public int cantidadMiscelaneo { get; private set; }
    public int cantidadTotal => cantidadHierro + cantidadOro + cantidadPlatino + cantidadMiscelaneo;

    public sistemaEspacial(string codigo, List<Asteroide> asteroides)
    {
        codigoSistema = codigo;
        Asteroides = asteroides;
    }

    public void procesarAsteroide()
    {
        foreach (var asteroide in Asteroides)
        {
            cantidadHierro += asteroide.Hierro;
            cantidadOro += asteroide.Oro;
            cantidadPlatino += asteroide.Platino;
            cantidadMiscelaneo += asteroide.Miscelaneo;
        }
    }

    public void generarReporte()
    {
        Console.WriteLine($"EN EL SISTEMA [{codigoSistema}] SE MINARON [{Asteroides.Count}] ASTEROIDES");
        Console.WriteLine($"{cantidadHierro} KG de hierro");
        Console.WriteLine($"{cantidadOro} KG de oro");
        Console.WriteLine($"{cantidadPlatino} KG de platino");
        Console.WriteLine($"{cantidadMiscelaneo} KG de metales misceláneos");
        Console.WriteLine($"Por un total de {cantidadTotal} KG de carga.");
    }

    public static sistemaEspacial generarSistemaRandom(Random random)
    {
        string codigoSistema = Guid.NewGuid().ToString().Substring(0, 8);
        int numAsteroides = random.Next(5, 11); // Generar de 5 a 10 asteroides
        
        List<Asteroide> asteroides = new List<Asteroide>();
        for (int i = 0; i < numAsteroides; i++)
        {
            Asteroide asteroide = Asteroide.generarAsteroideRandom(random);
            asteroides.Add(asteroide);
        }

        return new sistemaEspacial(codigoSistema, asteroides);
    }
}

class Asteroide
{
    public int Hierro { get; }
    public int Oro { get; }
    public int Platino { get; }
    public int Miscelaneo { get; }

    public Asteroide(int hierro, int oro, int platino, int miscelaneo)
    {
        Hierro = hierro;
        Oro = oro;
        Platino = platino;
        Miscelaneo = miscelaneo;
    }

    public static Asteroide generarAsteroideRandom(Random random)
    {
        int tamaño = random.Next(4); // tamaños
        int hierro = tamaño switch
        {
            0 => random.Next(251, 751),
            1 => random.Next(501, 1501),
            2 => random.Next(1251, 3751),
            _ => random.Next(2501, 7501),
        };
        int peso = tamaño switch
        {
            0 => 1000,
            1 => 2000,
            2 => 5000,
            _ => 10000,
        };
        int oro = random.Next(0, peso - hierro + 1);
        int platino = random.Next(0, peso - hierro - oro + 1);
        int miscelaneo = peso - hierro - oro - platino;

        return new Asteroide(hierro, oro, platino, miscelaneo);
    }
}
