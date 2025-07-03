<template>
  <div class="m-chart">
    <LineChartGenerator
      v-if="type === 'line'"
      :chart-options="chartOptions"
      :chart-data="chartData"
      :chart-id="chartId"
      :dataset-id-key="datasetIdKey"
      :plugins="plugins"
      :css-classes="cssClasses"
      :styles="styles"
      :width="width"
      :height="height"
    />
    <Bar
      v-if="type === 'bar'"
      :chart-options="chartOptions"
      :chart-data="chartData"
      :chart-id="chartId"
      :dataset-id-key="datasetIdKey"
      :plugins="plugins"
      :css-classes="cssClasses"
      :styles="styles"
      :width="width"
      :height="height"
    />
  </div>
</template>

<script>
// import { Bar } from 'vue-chartjs/legacy' // "/legacy"を忘れないように('vue-chartjs'がvue3用、'vue-chartjs/legacy'がvue2用)
import { Line as LineChartGenerator, Bar } from 'vue-chartjs/legacy'
import { Chart as ChartJS, registerables } from 'chart.js'
import chartjsPluginDatalabels from 'chartjs-plugin-datalabels'

ChartJS.register(...registerables)
ChartJS.register(chartjsPluginDatalabels)

export default {
  name: 'Chart',
  components: { LineChartGenerator, Bar },
  props: {
    type: {
      type: String,
      default: '',
    },
    chartId: {
      type: String,
      default: 'bar-chart',
    },
    datasetIdKey: {
      type: String,
      default: 'label',
    },
    width: {
      type: Number,
      default: 1000,
    },
    height: {
      type: Number,
      default: 400,
    },
    cssClasses: {
      default: '',
      type: String,
    },
    styles: {
      type: Object,
      default: () => {},
    },
    plugins: {
      type: Array,
      default: () => [],
    },
    chartData: {
      type: Object,
      default() {
        return {}
      },
    },
    chartOptions: {
      type: Object,
      default() {
        return {}
      },
    },
  },
  data() {
    return {
      data: {
        labels: ['Green', 'Red', 'Blue'],
        datasets: [
          {
            backgroundColor: ['#41B883', '#E46651', '#00D8FF'],
            data: [1, 10, 5],
          },
        ],
      },
      options: {
        plugins: {
          datalabels: {
            color: 'white',
            textAlign: 'center',
            font: {
              weight: 'bold',
              size: 16,
            },
          },
        },
      },
    }
  },
}
</script>
