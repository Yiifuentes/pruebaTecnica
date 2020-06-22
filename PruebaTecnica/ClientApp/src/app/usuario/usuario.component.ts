import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html'
})
export class UsuarioComponent {
  public listaUsuario: Usuario[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Usuario[]>(baseUrl + 'usuario').subscribe(result => {
      this.listaUsuario = result;
    }, error => console.error(error));
  }
}

interface Usuario {
  nombre: string;
  apellido: number;
  identificacion: number;
  email: string;
}
