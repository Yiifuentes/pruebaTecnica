import { Component, Inject } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html'
})
export class UsuarioComponent {
  public listaUsuario: Usuario[] = [];
  public respuesta: any;
  public formulario = new Usuario("", "", 0, 0, "", "");
  public crear: boolean = true;
  public editar: boolean = false;
  public borrar: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

    this.obtenerListaUsuario(http, baseUrl);
  }

  private obtenerListaUsuario(http: HttpClient, baseUrl: string) {
        http.get<Respuesta>(baseUrl + 'api/usuario/get').subscribe(result => {
            this.respuesta = result.message;
            result.listaUsuario.forEach((user: {
                nombre: any;
                apellido: any;
                email: any;
                identificacion: any;
                tipoIdentificacionId: any;

            }) => {
              var item = new Usuario(user.nombre, user.apellido, user.identificacion, user.tipoIdentificacionId, "", "" + user.email);
                this.listaUsuario.push(item);
            });
            console.log(this.listaUsuario);
        }, error => console.error(error));
    }

  crearUsuario() {
    this.postUsuario(this.formulario).subscribe(
      resp => {
        this.nuevoUsuario();
        this.listaUsuario = [];
        this.obtenerListaUsuario(this.http, this.baseUrl);
      },
      err => {
        console.log(err);
      }
    );
  }

  postUsuario(user: Usuario): Observable<HttpResponse<Usuario>> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post<Usuario>(this.baseUrl+'api/usuario/post', user,
      {
        headers: httpHeaders,
        observe: 'response'
      }
    );
  }

  seleccionEditar(selec: Usuario) {
    console.log(selec);
    this.formulario = selec;
    this.editar = true;
    this.borrar = true;
    this.crear = false;
  }

  putUsuario(user: Usuario): Observable<HttpResponse<Usuario>> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    const params = new HttpParams()
      .set('id', user.identificacion.toString());
    return this.http.post<Usuario>(this.baseUrl + 'api/usuario/put', user, 
      {
        headers: httpHeaders,
        observe: 'response'
      }
    );
  }
   

  editarUsuario() {
    this.putUsuario(this.formulario).subscribe(
      resp => {
        this.nuevoUsuario();
        this.listaUsuario = [];
        this.obtenerListaUsuario(this.http, this.baseUrl);
      },
      err => {
        console.log(err);
      }
    );

  }

  deleteUsuario(user: Usuario): Observable<HttpResponse<Usuario>> {
    let httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    const params = new HttpParams()
      .set('id', user.identificacion.toString());
    return this.http.post<Usuario>(this.baseUrl + 'api/usuario/delete', user,
      {
        headers: httpHeaders,
        observe: 'response'
      }
    );
  }
  removerUsuario() {
    this.deleteUsuario(this.formulario).subscribe(
      resp => {
        this.nuevoUsuario();
        this.listaUsuario = [];
        this.obtenerListaUsuario(this.http, this.baseUrl);
      },
      err => {
        console.log(err);
      }
    );
  }

  nuevoUsuario() {

    this.crear = true;
    this.borrar = false;
    this.editar = false;
     this.formulario = new Usuario("", "", 0, 0, "", "");
  }
}

export class Usuario {
  constructor(
    public nombre: string,
    public apellido: string,
    public identificacion: number,
    public tipoIdentificacionId: number,
    public passwordHash: string,
    public email: string,

  ){ }
}

interface Respuesta {
  listaUsuario: Usuario[];
  message: string;
  status: any;
} 
