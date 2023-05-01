import useDocumentTitle from "../useDocumentTitle";

export default function HomePage() {
    useDocumentTitle("Главная");

    return <div className="text-center">
        <h1 className="display-4">Приёмная комиссия</h1>
        <p>Здесь будет размещенная информация о приёмной комиссии.</p>
    </div>
;
}
