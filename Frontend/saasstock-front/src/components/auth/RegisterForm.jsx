import { useState } from 'react';
import { useAuth } from '../../context/AuthContext'; 

export default function RegisterForm({ onSwitchToLogin, onSuccess }) {
  const { register } = useAuth();
  const [username, setName] = useState('');
  const [companyName, setCompanyName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    if (!username || !companyName || !email || !password) {
      setError('Por favor, completá todos los campos.');
      setLoading(false);
      return;
    }

    const result = await register(username, email, password, companyName);
    
    if (result.success) {
      onSuccess(); // Aviso de que se registro con exito
    } else {
      setError(result.error);
    }
    setLoading(false);
  };

  //etiquetas con estilos para el modal del registro.
  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <h2 className="text-2xl font-bold text-center text-white">Crea tu cuenta en el sistema de stock y ventas</h2>
      <p className="text-sm text-center text-zinc-400">Comenzá a gestionar tu negocio </p>

      {error && (
        <div className="p-2 text-sm bg-red-950/50 border border-red-500/50 text-red-200 rounded">
          {error}
        </div>
      )}

      <div>
        <label className="block text-xs font-semibold uppercase text-zinc-400 mb-1">Nombre Completo</label>
        <input
          type="text"
          value={username}
          onChange={(e) => setName(e.target.value)}
          className="w-full p-2.5 rounded bg-zinc-900 border border-zinc-800 text-white focus:border-emerald-500 focus:outline-none transition-colors"
          placeholder="Lionel Messi"
        />
      </div>

      <div>
        <label className="block text-xs font-semibold uppercase text-zinc-400 mb-1">Nombre de tu Empresa </label>
        <input
          type="text"
          value={companyName}
          onChange={(e) => setCompanyName(e.target.value)}
          className="w-full p-2.5 rounded bg-zinc-900 border border-zinc-800 text-white focus:border-emerald-500 focus:outline-none transition-colors"
          placeholder="Distribuidora Messi"
        />
      </div>

      <div>
        <label className="block text-xs font-semibold uppercase text-zinc-400 mb-1">Correo electronico</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full p-2.5 rounded bg-zinc-900 border border-zinc-800 text-white focus:border-emerald-500 focus:outline-none transition-colors"
          placeholder="correo@empresa.com"
        />
      </div>

      <div>
        <label className="block text-xs font-semibold uppercase text-zinc-400 mb-1">Contraseña</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full p-2.5 rounded bg-zinc-900 border border-zinc-800 text-white focus:border-emerald-500 focus:outline-none transition-colors"
          placeholder="••••••••"
        />
      </div>

      <button
        type="submit"
        disabled={loading}
        className="w-full py-2.5 rounded bg-emerald-600 hover:bg-emerald-500 text-white font-semibold transition-colors disabled:opacity-50"
      >
        {loading ? 'Registrando...' : 'Completar registro'}
      </button>

      <p className="text-sm text-center text-zinc-500">
        ¿Ya tenés cuenta?{' '}
        <button
          type="button"
          onClick={onSwitchToLogin}
          className="text-emerald-400 hover:underline"
        >
          Iniciá sesión
        </button>
      </p>
    </form>
  );
}