using System;
using System.Collections.Generic;
using System.Linq;
using Argos.Data.Context;
using Hermes.Models;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Helpers
{
    public class Location
    {

        public static void getDistancias(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                var alertas = context.DbSetCameraLog.Include(i => i.Camera).AsEnumerable();
                var posicoesEfetivos = context.DbSetUsuarioPosicao.GroupBy(i => i.UsuarioId).Select(i => i.OrderByDescending(j => j.CriadoEm).FirstOrDefault()).AsEnumerable();

                foreach (var alerta in alertas)
                {
                    var coordenadaAlerta = new Geocoordinate(alerta.Camera.Latitude, alerta.Camera.Longitude);

                    List<UsuarioDistancia> listaDistancias = new List<UsuarioDistancia>(); 
                    foreach (var posicaoEfetivo in posicoesEfetivos)
                    {
                        var posicao = new Geocoordinate(posicaoEfetivo.Latitude, posicaoEfetivo.Longitude);

                        var distancia = coordenadaAlerta.GetDistanceTo(posicao);
                       
                        listaDistancias.Add(new UsuarioDistancia(distancia, posicaoEfetivo.UsuarioId));
                    }
                    
                    
                    var lista = listaDistancias.OrderByDescending(i => i.Distancia).Take(5); //adicionar ao env depois
                    foreach (var usuarioDistancia in lista)
                    {
                        var dispositivo = context.DbSetDispositivo.Where(i => i.UsuarioId == usuarioDistancia.UsuarioId).FirstOrDefault();
                        //implementar notificacoes do firebase
                    }
                }
            }
        }

    }
}