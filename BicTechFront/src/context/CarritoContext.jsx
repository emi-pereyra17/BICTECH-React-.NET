import { createContext, useContext, useEffect, useState } from "react";
import { AuthContext } from "./AuthContext";

export const CarritoContext = createContext();

export const CarritoProvider = ({ children }) => {
  const { usuario } = useContext(AuthContext);
  const [carrito, setCarrito] = useState([]);

  // Cargar carrito del backend al iniciar sesión
  useEffect(() => {
  if (usuario) {
    console.log(localStorage.getItem("token"));
    fetch(`http://localhost:5087/carritos/${usuario.id}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then(async (res) => {
        if (!res.ok) {
          // Opcional: puedes mostrar un mensaje de error aquí
          setCarrito([]);
          return;
        }
        const data = await res.json();
        setCarrito(data.CarritoDetalles || data.productos || []);
      });
  } else {
    setCarrito([]);
  }
}, [usuario]);

  // Agregar producto al carrito (llama al backend)
  const agregarAlCarrito = async (productoId, cantidad = 1) => {
    if (!usuario) return;
    const res = await fetch(
      `http://localhost:5087/carritos/${usuario.id}/productos/${productoId}/add?cantidad=${cantidad}`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      }
    );
    if (!res.ok) {
      const errorData = await res.json();
      throw new Error(errorData.message || "Error al agregar producto");
    }
    // Refresca el carrito
    const res2 = await fetch(`http://localhost:5087/carritos/${usuario.id}`);
    const data = await res2.json();
    setCarrito(data.CarritoDetalles || data.productos || []);
  };

  // Actualizar cantidad
  const actualizarCantidad = async (productoId, cantidad) => {
    if (!usuario) return;
    const res = await fetch(
      `http://localhost:5087/carritos/${usuario.id}/productos/${productoId}?cantidad=${cantidad}`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      }
    );
    if (!res.ok) {
      const errorData = await res.json();
      throw new Error(errorData.message || "Error al actualizar cantidad");
    }
    const res2 = await fetch(`http://localhost:5087/carritos/${usuario.id}`);
    const data = await res2.json();
    setCarrito(data.CarritoDetalles || data.productos || []);
  };

  // Quitar producto
  const quitarDelCarrito = async (productoId) => {
    if (!usuario) return;
    const res = await fetch(
      `http://localhost:5087/carritos/${usuario.id}/productos/${productoId}`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      }
    );
    if (!res.ok) {
      const errorData = await res.json();
      throw new Error(errorData.message || "Error al quitar producto");
    }
    const res2 = await fetch(`http://localhost:5087/carritos/${usuario.id}`);
    const data = await res2.json();
    setCarrito(data.CarritoDetalles || data.productos || []);
  };

  // Vaciar carrito
  const vaciarCarrito = async () => {
    if (!usuario) return;
    const res = await fetch(`http://localhost:5087/carritos/${usuario.id}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    });
    if (!res.ok) {
      const errorData = await res.json();
      throw new Error(errorData.message || "Error al vaciar carrito");
    }
    setCarrito([]);
  };

  return (
    <CarritoContext.Provider
      value={{
        carrito,
        agregarAlCarrito,
        actualizarCantidad,
        quitarDelCarrito,
        vaciarCarrito,
      }}
    >
      {children}
    </CarritoContext.Provider>
  );
};
