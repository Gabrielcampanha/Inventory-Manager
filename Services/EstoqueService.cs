using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using InventoryManager.Interfaces;
using InventoryManager.Models;

namespace InventoryManager.Services
{
    public class EstoqueService
    {
        private List<IEstoque> produtos = new List<IEstoque>();

        private enum Menu { Listar = 1, Adicionar, Remover, Entrada, Saida, Sair }

        public void Executar()
        {
            Carregar();
            bool sair = false;

            while (!sair)
            {
                Console.WriteLine("Sistema de Estoque");
                Console.WriteLine("1-Listar\n2-Adicionar\n3-Remover\n4-Entrada\n5-Saída\n6-Sair");

                if (!int.TryParse(Console.ReadLine(), out int opInt))
                    continue;

                if (opInt > 0 && opInt < 7)
                {
                    Menu escolha = (Menu)opInt;

                    switch (escolha)
                    {
                        case Menu.Listar: Listagem(); break;
                        case Menu.Adicionar: Cadastro(); break;
                        case Menu.Remover: Remover(); break;
                        case Menu.Entrada: Entrada(); break;
                        case Menu.Saida: Saida(); break;
                        case Menu.Sair: sair = true; break;
                    }
                }

                Console.Clear();
            }
        }

        private void Listagem()
        {
            Console.WriteLine("Listagem de produtos cadastrados:");

            for (int i = 0; i < produtos.Count; i++)
            {
                Console.WriteLine($"ID: {i}");
                produtos[i].Exibir();
            }

            Console.ReadLine();
        }

        private void Remover()
        {
            Listagem();
            Console.WriteLine("Digite o ID do elemento que você quer remover:");

            if (int.TryParse(Console.ReadLine(), out int id) &&
                id >= 0 && id < produtos.Count)
            {
                produtos.RemoveAt(id);
                Salvar();
            }
        }

        private void Entrada()
        {
            Listagem();
            Console.WriteLine("Digite o ID do elemento para entrada:");

            if (int.TryParse(Console.ReadLine(), out int id) &&
                id >= 0 && id < produtos.Count)
            {
                produtos[id].AdicionarEntrada();
                Salvar();
            }
        }

        private void Saida()
        {
            Listagem();
            Console.WriteLine("Digite o ID do elemento para saída:");

            if (int.TryParse(Console.ReadLine(), out int id) &&
                id >= 0 && id < produtos.Count)
            {
                produtos[id].AdicionarSaida();
                Salvar();
            }
        }

        private void Cadastro()
        {
            Console.WriteLine("Cadastro de Produto:");
            Console.WriteLine("1-Produto Físico\n2-Ebook\n3-Curso");

            if (!int.TryParse(Console.ReadLine(), out int escolha))
                return;

            switch (escolha)
            {
                case 1: CadastrarPFisico(); break;
                case 2: CadastrarEbook(); break;
                case 3: CadastrarCurso(); break;
            }
        }

        private void CadastrarPFisico()
        {
            Console.WriteLine("Nome:");
            string nome = Console.ReadLine();

            Console.WriteLine("Preço:");
            float preco = float.Parse(Console.ReadLine());

            Console.WriteLine("Frete:");
            float frete = float.Parse(Console.ReadLine());

            produtos.Add(new Produto_Fisico(nome, preco, frete));
            Salvar();
        }

        private void CadastrarEbook()
        {
            Console.WriteLine("Nome:");
            string nome = Console.ReadLine();

            Console.WriteLine("Preço:");
            float preco = float.Parse(Console.ReadLine());

            Console.WriteLine("Autor:");
            string autor = Console.ReadLine();

            produtos.Add(new Ebook(nome, preco, autor));
            Salvar();
        }

        private void CadastrarCurso()
        {
            Console.WriteLine("Nome:");
            string nome = Console.ReadLine();

            Console.WriteLine("Preço:");
            float preco = float.Parse(Console.ReadLine());

            Console.WriteLine("Autor:");
            string autor = Console.ReadLine();

            produtos.Add(new Curso(nome, preco, autor));
            Salvar();
        }

        private void Salvar()
        {
            FileStream stream = new FileStream("Produtos.dat", FileMode.OpenOrCreate);
            BinaryFormatter encoder = new BinaryFormatter();
            encoder.Serialize(stream, produtos);
            stream.Close();
        }

        private void Carregar()
        {
            if (!File.Exists("Produtos.dat"))
                return;

            FileStream stream = new FileStream("Produtos.dat", FileMode.Open);
            BinaryFormatter encoder = new BinaryFormatter();

            try
            {
                produtos = (List<IEstoque>)encoder.Deserialize(stream) ?? new List<IEstoque>();
            }
            catch
            {
                produtos = new List<IEstoque>();
            }
            stream.Close();
        }
    }
}