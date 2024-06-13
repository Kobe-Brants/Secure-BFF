export default function Footer() {
  return (
    <footer className="bg-white dark:bg-gray-900 py-3">
      <hr className="my-6 border-gray-200 sm:mx-auto dark:border-gray-700 lg:my-3" />
      <div className="sm:flex sm:items-center sm:justify-between">
        <span className="text-sm text-gray-500 sm:text-center dark:text-gray-400">
          © {new Date().getFullYear()}{' '}
          <a
            href="https://fransagro.be/"
            className="hover:underline text-yellow"
          >
            Frans Agro
          </a>
          . Made by{' '}
          <a
            href="https://www.linkedin.com/in/kobe-brants/"
            className="hover:underline text-yellow"
          >
            Kobe Brants
          </a>
          .
        </span>
      </div>
    </footer>
  );
}
