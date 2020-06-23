import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html'
})
export class UsuarioComponent {
  public listaUsuario: Usuario[] = [];
  public respuesta: any;
  public formulario = new Usuario("", "", 0, 0, "", "");

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Respuesta>(baseUrl + 'usuario').subscribe(result => {
      this.respuesta = result.message;
         
      result.listaUsuario.forEach(
        (user: { nombre: any; apellido: any; email: any }) => {
          var item = new Usuario("nombre" + user.nombre, "" + user.apellido, 1, 0, "", "" + user.email);
          this.listaUsuario.push(item);          
        }
      )
 
      console.log(this.listaUsuario);
    }, error => console.error(error));
  }


}

export class Usuario {
  constructor(
    public nombre: string,
    public apellido: string,
    public identificacion: number,
    public tipoIdentificacion: number,
    public clave: string,
    public email: string,

  ){ }
}

interface Respuesta {
  listaUsuario: Usuario[];
  message: string;
  status: any;
} 
