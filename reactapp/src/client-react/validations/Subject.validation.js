import * as yup from 'yup';

export const SubjectValidationSchema = yup.object().shape({
    name: yup.string().required('Введите название'),
});