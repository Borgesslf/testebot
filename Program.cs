using System;

// Interface de manutenção
public interface IManutencao
{
    decimal CalcularCustoRevisao();
}

// Classe base abstrata Veiculo
public abstract class Veiculo : IManutencao
{
    public string Placa { get; private set; }
    protected decimal Tanque;

    protected Veiculo(string placa, decimal tanque)
    {
        if (tanque < 0)
            throw new ArgumentException("O tanque não pode ser negativo.");
        Placa = placa;
        Tanque = tanque;
    }

    public abstract void Viajar(decimal distanciaKm);
    public abstract decimal CalcularPedagio();
    public abstract decimal CalcularCustoRevisao();
}

// Classe Carro
public class Carro : Veiculo
{
    public Carro(string placa, decimal tanque) : base(placa, tanque) { }

    public override void Viajar(decimal distanciaKm)
    {
        decimal litrosGastados = distanciaKm / 10m;
        if (Tanque - litrosGastados < 0)
            throw new InvalidOperationException("Combustível insuficiente para o carro.");
        Tanque -= litrosGastados;
    }

    public override decimal CalcularPedagio() => 8.50m;
    public override decimal CalcularCustoRevisao() => 300.00m;
}

// Classe Caminhao
public class Caminhao : Veiculo
{
    public int QuantidadeEixos { get; private set; }

    public Caminhao(string placa, decimal tanque, int quantidadeEixos) : base(placa, tanque)
    {
        QuantidadeEixos = quantidadeEixos;
    }

    public override void Viajar(decimal distanciaKm)
    {
        decimal litrosGastados = distanciaKm / 4m;
        if (Tanque - litrosGastados < 0)
            throw new InvalidOperationException("Combustível insuficiente para o caminhão.");
        Tanque -= litrosGastados;
    }

    public override decimal CalcularPedagio() => 8.50m * QuantidadeEixos;
    public override decimal CalcularCustoRevisao() => 1500.00m;
}

class Program
{
    /* MÁGICA DO POLIMORFISMO: Este método aceita QUALQUER veículo (Carro ou Caminhão) */
    static void SimularViagemPolimorfica(Veiculo v, decimal quilometros)
    {
        try
        {
            Console.WriteLine($"\n--- A INICIAR VIAGEM COM O VEÍCULO: {v.Placa} ---");
            v.Viajar(quilometros);
            Console.WriteLine($"Sucesso! Restou no tanque: {v.Tanque:F2} Litros");
            Console.WriteLine($"Custo de Portagem (Pedágio): {v.CalcularPedagio():C}");
            Console.WriteLine($"Previsão de Revisão na Oficina: {v.CalcularCustoRevisao():C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ALERTA NA VIAGEM: {ex.Message}");
        }
    }
    
    static void Main()
    {
        Console.WriteLine("--- CADASTRO DA FROTA ---");
        
        /* 1. Cadastrando o Carro */
        Console.WriteLine("\n[A] Configurar o Carro do Gerente");
        Console.Write("Digite a Placa: ");
        string placaCarro = Console.ReadLine();
        Console.Write("Combustível Inicial (Litros): ");
        decimal tanqueCarro = decimal.Parse(Console.ReadLine());
        
        Carro carroGerente = null;
        try
        {
            carroGerente = new Carro(placaCarro, tanqueCarro);
            Console.WriteLine("Carro registado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO NO REGISTO: {ex.Message}");
        }
        
        /* 2. Cadastrando o Caminhão */
        Console.WriteLine("\n[B] Configurar o Caminhão de Carga");
        Console.Write("Digite a Placa: ");
        string placaCam = Console.ReadLine();
        Console.Write("Combustível Inicial (Litros): ");
        decimal tanqueCam = decimal.Parse(Console.ReadLine());
        Console.Write("Quantidade de Eixos: ");
        int eixosCam = int.Parse(Console.ReadLine());
        
        Caminhao caminhaoCarga = null;
        try
        {
            caminhaoCarga = new Caminhao(placaCam, tanqueCam, eixosCam);
            Console.WriteLine("Caminhão registado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO NO REGISTO: {ex.Message}");
        }
        
        /* 3. A Simulação da Viagem */
        Console.WriteLine("\n--- SIMULAÇÃO DE ROTA ---");
        Console.Write("Qual a distância do trajeto (Km)? ");
        decimal distancia = decimal.Parse(Console.ReadLine());
        
        /* Enviamos os dois veículos diferentes para o mesmo método polimórfico */
        if (carroGerente != null)
        {
            SimularViagemPolimorfica(carroGerente, distancia);
        }
        
        if (caminhaoCarga != null)
        {
            SimularViagemPolimorfica(caminhaoCarga, distancia);
        }
        
        Console.WriteLine("\nSimulação concluída. Pressione ENTER para sair.");
        Console.ReadLine();
    }
}