import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from 'primevue/column';
import InputText from "primevue/inputtext";
import './index.css'
import Fieldset from "primevue/fieldset";
import ToggleSwitch from "primevue/toggleswitch";

const app = createApp(App);
app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
});

app.component("Button", Button);
app.component("DataTable", DataTable);
app.component("Column", Column);
app.component("InputText", InputText);
app.component("Fieldset", Fieldset);
app.component("ToggleSwitch", ToggleSwitch);

app.mount('#app');