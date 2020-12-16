using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argos.Data.Context;
using Argos.Domain.CameraLogRoot;
using Hermes.Models;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Helpers
{
    public class Location
    {

        public static async Task getDistancias(DbContextOptions<DataContext> options, IEnumerable<CameraLog> alertas)
        {
            using (var context = new DataContext(options))
            {
                var posicoesEfetivos = context.DbSetUsuarioPosicao.GroupBy(i => i.UsuarioId).Select(i => i.OrderByDescending(j => j.CriadoEm).FirstOrDefault()).AsEnumerable();

                foreach (var alerta in alertas)
                {
                    var coordenadaAlerta = new Geocoordinate(alerta.Camera.Latitude, alerta.Camera.Longitude);

                    List<UsuarioDistancia> listaDistancias = new List<UsuarioDistancia>(); 
                    foreach (var posicaoEfetivo in posicoesEfetivos)
                    {
                        var posicao = new Geocoordinate(posicaoEfetivo.Latitude, posicaoEfetivo.Longitude);

                        var distancia = coordenadaAlerta.GetDistanceTo(posicao);
                       
                        var usuario = context.DbSetUsuario.Where(i => i.Id == posicaoEfetivo.UsuarioId).FirstOrDefault();
                        listaDistancias.Add(new UsuarioDistancia(distancia, posicaoEfetivo.UsuarioId, usuario.TokenFirebase));
                    }
                    
                    
                    var lista = listaDistancias.OrderByDescending(i => i.Distancia).Take(5); //adicionar ao env depois
                    foreach (var usuarioDistancia in lista)
                    {
                        var distancia = (usuarioDistancia.Distancia / 1000);
                        var notificacao = new Notification()
                        {
                            title = "Alerta",
                            text = "Foi emitido um alerta de "+ alerta.Alerta.Tipo.Descricao + " para " + alerta.Alerta.Placa + " há " + distancia.ToString("F") + " quilômetros de você."
                        };
                        var data = new AlertPayload()
                        {
                            id = alerta.Alerta.Id,
                            placa = alerta.Alerta.Placa,
                            distancia = distancia.ToString("F") + " km"
                        };

                        try
                        {
                            await SendPushNotifications.send(usuarioDistancia.Token, notificacao, data);
                        }
                        catch(Exception e)
                        {
                            
                        }
                        
                    }

                    alerta.Status = 1;
                    context.DbSetCameraLog.Update(alerta);
                    context.SaveChanges();
                }
            }
        }

    }
}