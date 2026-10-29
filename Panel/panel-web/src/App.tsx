
import { useEffect, useState } from "react";
import "./App.css";

type Comic = {
  id: number;
  title: string;
  synopsis: string;
  coverImageUrl: string;
  author: string;
  status: string;
  genres: Array<string>
};

function App() {
  const [comics, setComics] = useState<Comic[]>([]);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadComics() {
      try {
        const url = "http://localhost:5023/api/comics";

        const response = await fetch(url);

        console.log("API status:", response.status);

        if (!response.ok) {
          throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }

        const data = await response.json();
        console.log("Comics received:", data);

        setComics(data);
        setError("");
      } catch (err) {
        console.error("Failed to load comics:", err);
        setError(err instanceof Error ? err.message : String(err));
      }
    }

    loadComics();
  }, []);


  return (
    <main>
      <h1>Panel</h1>
      <p>Discover your next favorite comic.</p>

      {error && <p role="alert">{error}</p>}

      <section className="comic-grid">
        {comics.map((comic) => (
          <article key={comic.id} className="comic-card">
            <img
              src={comic.coverImageUrl}
              alt={`${comic.title} cover`}
            />
            <h2>{comic.title}</h2>
            <p>{comic.synopsis}</p>
            <small>{comic.author} · {comic.status}</small>
            <div className="comic-genres">
  {comic.genres.map((genre) => (
    <span key={genre} className="genre-tag">
      {genre}
    </span>
  ))}
</div>
          </article>
        ))}
      </section>
    </main>
  );
}

export default App;
